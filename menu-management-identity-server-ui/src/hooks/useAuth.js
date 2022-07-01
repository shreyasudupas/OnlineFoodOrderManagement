import { createContext, useContext,Component } from "react"
import AuthService from "../utilities/AuthService";

const AuthContext = createContext({
    signinRedirectCallback: () => ({}),
    logout: () => ({}),
    signoutRedirectCallback: () => ({}),
    isAuthenticated: () => ({}),
    signinRedirect: () => ({}),
    createSigninRequest: () => ({})
})

// export const AuthProvider = ({ children }) => {
//     //const [user,setUser] = useState(null)

//     // useEffect(()=>{
//     //     let user = localStorage.getItem(Constants.LOGIN_LOCAL_STORAGE_NAME)
//     //     setUser(user)
//     // },[])

//     // const login = (user) => {
//     //     setUser(user)
//     // }

//     // const logout = () => {
//     //     setUser(null)
//     // }

//     // const value = useMemo(
//     //     () => ({
//     //       user,
//     //       login,
//     //       logout
//     //     }),
//     //     [user]
//     //   );

//     return (
//         <AuthContext.Provider value={authService}>
//             {children}
//         </AuthContext.Provider>
//     )
// }

export class AuthProvider extends Component {
    authService;
    constructor(props) {
        super(props);
        this.authService = new AuthService();
    }
    render() {
        return <AuthContext.Provider value={this.authService}>{this.props.children}</AuthContext.Provider>;
    }
}

//custom hooks
export const useAuth = () => {
    return useContext(AuthContext)
}