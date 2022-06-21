import { createContext, useContext, useEffect, useMemo, useState } from "react"
import { Constants } from '../utilities/Constants';

const AuthContext = createContext(null)

export const AuthProvider = ({ children }) => {
    const [user,setUser] = useState(null)

    useEffect(()=>{
        let user = localStorage.getItem(Constants.LOGIN_LOCAL_STORAGE_NAME)
        setUser(user)
    },[])

    const login = (user) => {
        setUser(user)
    }

    const logout = () => {
        setUser(null)
    }

    const value = useMemo(
        () => ({
          user,
          login,
          logout
        }),
        [user]
      );

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    )
}

//custom hooks
export const useAuth = () => {
    return useContext(AuthContext)
}