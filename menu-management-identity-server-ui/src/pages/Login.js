import React, { useEffect, useReducer, useState } from 'react'
import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Password } from 'primereact/password';
import { Button } from 'primereact/button';
import { useLocation, useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { Constants } from '../utilities/Constants';
import axios from 'axios';

const initialState = {
    loading:false,
    error:'',
    username:'',
    password:'',
    result:{
        redirectRequired:null,
        error:null
    }
}

const loginReducer = (state,action) => {
    switch(action.type){
        case 'HANDLE_INPUT_TEXT':
            console.log(state)
            return {
                ...state,
                [action.feild]:action.value
            }
        case 'CALL_LOGIN_API':
            return {
                ...state,
                loading: true,
            }
        case 'VERIFY_LOGIN_DETAILS':
            return {
                ...state,
                loading: false,
                result: action.payload
            }
        case 'LOGIN_DETAILS_INCORRECT':
            return {
                ...state,
                loading: false,
                result: action.error
            }
        default:
            return state
    }
}

function Login() {

const [state,dispatch] = useReducer(loginReducer,initialState)
const navigate = useNavigate()
const auth = useAuth() 
const location = useLocation()
const [searchParams] = useSearchParams()
let returnUrl = searchParams.get('returnUrl')
const LOGIN_URL = "https://localhost:5006/api/Authentication/Login"

const redirectPath = location.state?.path || '/user'

const handleTextInput = (e) => {
    dispatch({
        type:'HANDLE_INPUT_TEXT',
        feild: e.target.name,
        value: e.target.value
    })
}

useEffect(()=>{
    if(state.result.error !== undefined && state.result.redirectRequired !== null){

        if(state.result.error != null){
            console.log(state.result.error)
            return
        }

        auth.login(state.username)
        localStorage.setItem( Constants.LOGIN_LOCAL_STORAGE_NAME,state.username)

        if(state.result.redirectRequired === true)
        {
            window.location.replace(returnUrl)
        }else{
            navigate(redirectPath,{ replace:true })
        }
    }

},[state.result])



const loginHandler = (e) => {
    e.preventDefault()
    //console.log(loginInfo)

    if(state.username !== undefined && state.username !== ''){

        const body = {
            'username':state.username,
            'password':state.password,
            'redirectUrl':returnUrl
        }

        dispatch({ type: 'CALL_LOGIN_API'})

        axios.post(LOGIN_URL,body)
        .then(res=> {
            let data = res.data
            console.log(`Login API Result: ${data}`)
            
            dispatch({ type: 'VERIFY_LOGIN_DETAILS',payload: data})
        })
        .catch(error => {
            dispatch({ type: 'LOGIN_DETAILS_INCORRECT',error: error})
        })
        
    }
}

  return (
    <div>
        {state.loading? 'Loading....':
        <Card title="Login">
        <form onSubmit={loginHandler}>
            <div className='field grid'>
                <label className='col-12 mb-2 md:col-2 md:mb-0' htmlFor="username">Enter your username</label>
                <InputText
                        id='username'
                        type='text' 
                        name="username" 
                        className='w-full'
                        value = {state.username}
                        onChange={(e)=> handleTextInput(e)}
                        />
                
            </div>
            <div className='field grid'>
                <label className='col-12 mb-2 md:col-2 md:mb-0' htmlFor="password">Enter your password</label>
                <Password id='password'
                        name="password" 
                        className='w-full'
                        value = {state.password}
                        onChange={(e)=> handleTextInput(e)}
                        />
            </div>
            <Button label="Submit" aria-label="Submit" type="submit" />
        </form>
        </Card> }
    </div>
    
  )
}

export default Login