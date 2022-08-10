import React,{ useEffect, useReducer, useState } from 'react'
import { Button } from 'primereact/button';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { InputTextarea } from 'primereact/inputtextarea';
import { Dropdown } from 'primereact/dropdown';
import { Checkbox } from 'primereact/checkbox';
import { Card } from 'primereact/card'
import axios from 'axios';

const initialState = {
    cities:[],
    areas:[],
    listUserAddress:[]
}

const reducer = (state,action) => {
    switch(action.type){
        case 'modify-cities':
            return {
                ...state,
                listUserAddress: state.listUserAddress.map((user)=>
                user["id"] === action.userId? {...user,myCities:action.value}:{...user})
            }
        case 'modify-area':
            return {
                ...state,
                listUserAddress: state.listUserAddress.map((user)=>
                user["id"] === action.userId? {...user,myAreas:action.value}:{...user})
            }
        case 'add-list-userAddress':
            return {
                ...state,
                listUserAddress:action.listUserAddress
            }
        case 'add-new-address':
            debugger;
            return {
                ...state,
                listUserAddress:[...state.listUserAddress,action.newAddress]
            }
        case 'modify-user-address-form':
            return {
                ...state,
                listUserAddress: state.listUserAddress.map((user)=>
                user["id"] === action.userId? {...user,[action.field]:action.value}:{...user})
            }
        default: 
            return state
    }
}

function UserAddress({getUserAddress,getActiveIndex}) {
    //console.log('User Address called'+ userAddress )
    //const [checked, setChecked] = useState(false)
    const [state, dispatch] = useReducer(reducer, initialState)
    //console.log(state.listUserAddress)
    let activeIndex = 0
    let newActive = null
    const GETCITIES_BYID = "https://localhost:5006/api/utility/getCityByStateId?StateId="
    const GETAREAS_BY_ID = "https://localhost:5006/api/utility/getAreaByCityId?CityId="

    useEffect(()=>{

        if(getUserAddress != null){
            dispatch({type:'add-list-userAddress',listUserAddress: getUserAddress})
        }
    }
    ,[getUserAddress,getActiveIndex])

    const addNewUserAddress = () =>{
        newActive = true
        debugger
        const newUser = { 
            id:0,
            fullAddress:'',
            city:'',
            citId:null,
            state:'',
            stateId:null,
            area:'',
            areaId:null,
            isActive:false,
            myCities:null,
            myAreas:null, 
            myStates: state.listUserAddress[0].myStates
        }
        dispatch({type:'add-new-address',newAddress:newUser})
    }

    const handleInput = (event,userId) => {
        dispatch({type:'modify-user-address-form', field:event.target.name, value:event.target.value ,userId: userId })
    }

    const handleCitiesInput = (event,userId) => {
        dispatch({type:'modify-user-address-form', field:event.target.name, value:event.target.value ,userId: userId })

        let url = GETCITIES_BYID + event.target.value
        axios.get(url)
        .then((result)=>{
            let response = result.data
            
            dispatch({type:'modify-cities',value: response,userId: userId})
        })
        .catch(err=>console.log(err))
    }

    const handleAreasInput = (event,userId) => {
        dispatch({type:'modify-user-address-form', field:event.target.name, value:event.target.value ,userId: userId })

        let url = GETAREAS_BY_ID + event.target.value
        axios.get(url)
        .then((result)=>{
            let response = result.data
            
            dispatch({type:'modify-area',value: response,userId: userId})
        })
        .catch(err=>console.log(err))
    }

    const saveUserAddress = (user) => {
        console.log(user)
    }

    const footer = <Button label="Add" icon="pi pi-plus" style={{marginRight: '.25em'}} onClick={()=> addNewUserAddress()}/>

    if ((getUserAddress === null) || (getUserAddress === undefined)) {
        return <p>Loading...</p>
    }
    else
        return (
            <React.Fragment>
                <Card title="Address" footer={footer}>
                    <div className='grid'>
                        <div className='col-12'>
                            <Accordion activeIndex={getActiveIndex}>
                                {state.listUserAddress.map((user, index) => {
                                    let accTabname = "Address " + (++index);

                                    if (user.isActive)
                                        activeIndex = index //find if the user address is active then open the accordian
                                    else if (user.isActive === true && newActive === true) //for new user newActive will be true so override the active Index
                                        activeIndex = index

                                    return (
                                        <AccordionTab header={accTabname} key={user.id}>
                                            <div className='grid'>
                                                <div className='col-12'>
                                                    <h5>Full Address</h5>
                                                    <InputTextarea
                                                        name="fullAddress"
                                                        value={user.fullAddress}
                                                        onChange={(e) => handleInput(e, user.id)}
                                                        className='w-full'
                                                        rows={5}
                                                        cols={30} />
                                                </div>
                                                <div className='col-12'>
                                                    <div className="field-checkbox">
                                                        <Checkbox inputId="acticeAddress"
                                                            name="isActive"
                                                            onChange={(e) => handleInput(e, user.id)}
                                                            checked={user.isActive}></Checkbox>
                                                        <label htmlFor="acticeAddress">IsActive</label>
                                                    </div>
                                                </div>
                                                <div className='col-12'>
                                                    <h5>State</h5>
                                                    <Dropdown
                                                        name="stateId"
                                                        value={user.stateId}
                                                        options={user.myStates}
                                                        optionValue="value"
                                                        optionLabel="label"
                                                        onChange={(e) => handleCitiesInput(e, user.id)}
                                                    />
                                                </div>
                                                <div className='col-12'>
                                                    <h5>City</h5>
                                                    <Dropdown
                                                        name="cityId"
                                                        value={user.cityId}
                                                        options={user.myCities}
                                                        optionValue="value"
                                                        optionLabel="label"
                                                        onChange={(e) => handleAreasInput(e, user.id)}
                                                    />
                                                </div>
                                                <div className='col-12'>
                                                    <h5>Area</h5>
                                                    <Dropdown
                                                        name="areaId"
                                                        value={user.areaId}
                                                        options={user.myAreas}
                                                        optionValue="value"
                                                        optionLabel="label"
                                                        onChange={(e) => handleInput(e, user.id)}
                                                    />
                                                </div>
                                                <div className='col-12'>
                                                    <Button label="Save" onClick={() => saveUserAddress(user)} />
                                                </div>
                                            </div>
                                        </AccordionTab>
                                    )
                                })}
                            </Accordion>
                        </div>
                    </div>
                </Card>

            </React.Fragment>
        )
}

export default UserAddress