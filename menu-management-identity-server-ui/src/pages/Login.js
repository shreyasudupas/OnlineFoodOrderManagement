import React, { useState } from 'react'
import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Password } from 'primereact/password';
import { Button } from 'primereact/button';
import { useLocation, useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { Constants } from '../utilities/Constants';

function Login() {
const [loginInfo,setLoginInfo] = useState({
    username:'',
    password:''
})
const navigate = useNavigate()
const auth = useAuth() 
const location = useLocation()
const [searchParams] = useSearchParams()
let returnUrl = searchParams.get('returnUrl')


const redirectPath = location.state?.path || '/user'

const loginHandler = (e) => {
    e.preventDefault()
    console.log(loginInfo)

    if(loginInfo !== null && loginInfo !== undefined){
        auth.login(loginInfo.username)

        if(!returnUrl){
            localStorage.setItem( Constants.LOGIN_LOCAL_STORAGE_NAME,loginInfo.username)
            navigate(redirectPath,{ replace:true })
        }else{
            window.location.replace(returnUrl)
        }
        
    }
}
  return (
    <Card title="Login">
        <form onSubmit={loginHandler}>
            <div className='field grid'>
                <label className='col-12 mb-2 md:col-2 md:mb-0' htmlFor="username">Enter your username</label>
                <InputText
                        type='text' 
                        id="username" 
                        className='w-full'
                        value={loginInfo.username} 
                        onChange={(e) => setLoginInfo({...loginInfo,username:e.target.value})} 
                        />
                
            </div>
            <div className='field grid'>
                <label className='col-12 mb-2 md:col-2 md:mb-0' htmlFor="password">Enter your password</label>
                <Password id="password" 
                        className='w-full'
                        value={loginInfo.password} 
                        onChange={(e) => setLoginInfo({...loginInfo,password:e.target.value})} 
                        />
            </div>
            <Button label="Submit" aria-label="Submit" type="submit" />
        </form>
    </Card>
  )
}

export default Login