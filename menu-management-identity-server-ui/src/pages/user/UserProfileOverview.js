import React, { useEffect, useReducer, useState } from 'react'
import { useAuth } from '../../hooks/useAuth'
import { Card } from 'primereact/card'
import { InputText } from 'primereact/inputtext';
import { InputNumber } from 'primereact/inputnumber';
import { Checkbox } from 'primereact/checkbox';
import '../../components/App.css'
import { Button } from 'primereact/button';
import UserClaim from '../../components/user/UserClaim';
import ImageUpload from '../../components/ImageUpload';
import UserAddress from '../../components/user/UserAddress';
import { Image } from 'primereact/image';
import { useQuery } from '@apollo/client';
import { GET_USERINFORMATION } from '../../graphQL/queries/GetUserInformation';

const initialState = {
  userId:null,
  userInformation:{ id:null,userName:'',email:'',cartAmount:0,points:0,address:[],claims:[] },
  activeIndex:0
}

const reducer = (state,action) => {
  switch(action.type){
    case 'UPDATE-USERID':
      return {
        ...state,
        userId: action.Id
      }
    case 'INITILIZE-USERINFORMATION':
      return {
        ...state,
        userInformation: action.userInfo
      }
    case 'UPDATE-USERINFORMATION':
      return {
        ...state,
        userInformation: {...state,[action.field]:action.value}
      }
    case 'UPDATE-ACTIVE-INDEX-ADDRESS':
      return {
        ...state,
        activeIndex: state.userInformation.address.findIndex((element)=> element.isActive === true)
      }
    default:
      return state
  }
}

function UserProfileOverview() {

  const [state,dispatch] = useReducer(reducer,initialState)
  const getUserContext = useAuth()
  let user = null;
  const { loading, error, data } = useQuery(GET_USERINFORMATION,{
    variables: {
      userId: state.userId
    }
  });

  //once the promise gets the value then
  useEffect(()=>{

    async function getUserFromToken(){
      user = await getUserContext.getUser()

      if(user !== null){
        dispatch({type:'UPDATE-USERID',Id: user["profile"].userId})
      }
    }

    getUserFromToken()

    if((data !== null) && (data !== undefined)){
      dispatch({type:'INITILIZE-USERINFORMATION',userInfo:data.userInformation})

      dispatch({type:'UPDATE-ACTIVE-INDEX-ADDRESS'})
    }
  }
  ,[user,data])

  const [checked, setChecked] = useState(false)

  //console.log( 'userInformation' + state.userInformation)

  const handleInput = (event) => {
    dispatch({type:'UPDATE-USERINFORMATION', field: event.target.name , value: event.target.value})
  }

  const saveUserInformation = (userInfo) => {
    console.log(userInfo)
  }

  //console.log('User overview called')
  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error :</p>;
  
  if (state.userInformation !== undefined) {
    return (
      <Card title="Welcome to the Content Management Dashboard" subTitle="User Profile">
        <div className='grid p-fluid'>
          <div className='col-5'>
            <Image src="https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png" alt="Image" width="250" preview />
          </div>
          <div className='col-7'>
            <div className='col-12'>
              <h5>Username</h5>
              <div className="p-inputgroup">
                <span className="p-inputgroup-addon">
                  <i className="pi pi-user"></i>
                </span>
                <InputText
                  name="userName"
                  value={state.userInformation.userName}
                  onChange={(e) => handleInput(e)}
                  placeholder="Username" />
              </div>
            </div>
            <div className='col-12'>
              <h5>Email</h5>
              <div className="p-inputgroup">
                <span className="p-inputgroup-addon">www</span>
                <InputText
                  name="email"
                  value={state.userInformation.email}
                  placeholder="Email"
                  onChange={(e) => handleInput(e)} />
              </div>
            </div>
            <div className='col-12'>
              <h5>Cart Ammount</h5>
              <div className="p-inputgroup">
                <span className="p-inputgroup-addon">$</span>
                <InputNumber
                  name="cartAmount"
                  placeholder="CartAmount"
                  value={state.userInformation.cartAmount}
                  onChange={(e) => handleInput(e)} />
              </div>
            </div>
            <div className='col-12'>
              <h5>Points</h5>
              <div className="p-inputgroup">
                <span className="p-inputgroup-addon">$</span>
                <InputNumber
                  name="points"
                  placeholder="Points"
                  value={state.userInformation.points}
                  onChange={(e) => handleInput(e)}
                />
              </div>
            </div>
            <div className='col-12'>
              <div className="field-checkbox">
                <Checkbox
                  name="active"
                  inputId="binary"
                  onChange={(e) => handleInput(e)}
                  checked={checked}></Checkbox>
                <label htmlFor="binary">Active</label>
              </div>
            </div>
          </div>
          <div className='p-col-4 col-offset-5'>
            <Button label="Save" className="p-button-rounded" onClick={ ()=>saveUserInformation(state.userInformation) }/>
          </div>
        </div>

        <div className='grid p-fluid'>
          <div className='p-col-12 w-full'>
            <ImageUpload />
          </div>
        </div>

        <div className="grid p-fluid App-Logo">
          <UserClaim userClaims={state.userInformation.claims} />
        </div>

        <UserAddress getUserAddress={state.userInformation.address} getActiveIndex={state.activeIndex} />

      </Card>
    )
  }
  else{
    return <p>Loading UserInformation....</p>
  }
  
}

export default UserProfileOverview