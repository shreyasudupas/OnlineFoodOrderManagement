import React, { useEffect, useReducer } from 'react'
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';

const initialState = {
    editClaimDialog:false,
    addClaimDialog:false,
    deleteConfirmDialog:false,
    claims:[],
    claim:{ 'key':null , 'value':null}
}

const reducer =(state,action) => {
    switch(action.type){
        case "edit-claim-dialog":
            return {
                ...state,
                editClaimDialog:action.isOpen
            }
        case "add-claim-dialog":
            return {
                ...state,
                addClaimDialog:action.isOpen
            }
        case "delete-claim-dialog":
            return {
                ...state,
                deleteConfirmDialog:action.isOpen
            }
        case "claims-list-update":
            return {
                ...state,
                claims: action.claims
            }
        case "claim-user-add":
            //console.log("custome action",action)
            return {
                ...state,
                claims: [...state.claims,action.addClaim]
            }
        case "claim-user-update":
            return {
                ...state,
                claim:action.claim
            }
        default: 
            return state
    }
}

function UserClaim({userClaims}) {
    const [state, dispatch] = useReducer(reducer,initialState)
   

    useEffect(()=>{
        if(userClaims != null){
            console.log('called userclaims')
            dispatch({type:'claims-list-update',claims:userClaims})
        }
    },[userClaims])


    const claimTypes = [
        { label: 'username', value: 'username' },
        { label: 'email', value: 'email' },
        { label: 'role', value: 'role' },
        { label: 'picture', value: 'picture' }
    ];


    const onClick = (name) => {
        dispatch({type: name ,isOpen:true })
    }

    const onHide = (name,save) => {
        dispatch({type: name ,isOpen:false })

        if(name==="delete-claim-dialog" && save === true){
            dispatch({type:'claims-list-update',claims: state.claims.filter(item=> item.key !== state.claim["key"])})
        }

        //edit operation
        if(name==="edit-claim-dialog" && save === true){
            dispatch({type:'claims-list-update',claims: state.claims.map((claim)=>
            claim.key === state.claim["key"]? {...claim,value:state.claim["value"]} : {...claim})})
        }

        //add operation
        if(name==="add-claim-dialog" && save === true){
            //state.claims.push({key:state.claim['key'],value:state.claim['value']})
            dispatch({type:'claim-user-add',addClaim: state.claim})
        }

        dispatch({type:'claim-user-update',claim:{type:'null',value:'null'}})
    }

    const renderFooter = (name) => {
        return (
            <div>
                <Button label="No" icon="pi pi-times" onClick={() => onHide(name,false)} className="p-button-text" />
                <Button label="Save" icon="pi pi-check" onClick={() => onHide(name,true)} autoFocus />
            </div>
        );
    }

    //Edit Dialog operation
    const oprationAddTemplate = (rowData) => {
        return <Button icon="pi pi-user-edit" onClick={() => modifyClaim(rowData) }/>
    }

    const modifyClaim = (rowData) => {
        //console.log(rowData)
        dispatch({type:'claim-user-update',claim:rowData})
        //console.log(userClaim)
        onClick('edit-claim-dialog')
    }

    //Delete Opertaions
    const oprationDeleteTemplate = (rowData) => {
        return <Button icon="pi pi-user-minus" onClick={() => deleteClaim(rowData)}/>
    }

    const deleteClaim = (rowData) => {
        dispatch({type:'claim-user-update',claim:rowData})
        onClick('delete-claim-dialog')
    }

    //Dialog change of UserClaim for add/Edit
    const handleClaimValueChange = (event) =>{
        dispatch({type:'claim-user-update',claim: {...state.claim,value:event.target.value}})
    }
    const handleClaimTypeChange = (event) =>{
        dispatch({type:'claim-user-update',claim: {...state.claim,key:event.target.value}})
    }

    //called when add is clicked
    const addPreUserClaim = () =>{
        onClick('add-claim-dialog')
    }

  return (
    <>
    <div className='col-12'>
        <DataTable header="User Claims" value={state.claims} responsiveLayout="scroll" className='w-full'>
            <Column field="label" header="Claim Type"></Column>
            <Column field="value" header="Claim Name"></Column>
            <Column field='edit' header="Edit" body={oprationAddTemplate}></Column>
            <Column field='delete' header="Delete" body={oprationDeleteTemplate}></Column>
        </DataTable>
    </div>
    <div className='col-4 col-offset-8'>
        <Button label="Add" className="p-button-raised" onClick={() => addPreUserClaim()}/>
    </div>
    {/* <div className='col-6'>
        <Button label="Delete" className="p-button-danger" />
    </div> */}
    <Dialog header="User Claim" visible={state.editClaimDialog} style={{ width: '50vw' }} footer={renderFooter('edit-claim-dialog')}
     onHide={() => onHide('edit-claim-dialog',false)}>
            <div className='grid'>
                <div className='col-6'>
                    <h5>Key</h5>
                    <Dropdown
                        value={state.claim.label} 
                        options={claimTypes} 
                        optionValue="value"
                        optionLabel="label" 
                        onChange={e=> handleClaimTypeChange(e)}
                        placeholder="Select a User Type" />
                </div>
                <div className='col-6'>
                <h5>Value</h5>
                <InputText 
                    onChange={e=> handleClaimValueChange(e)} 
                    defaultValue={state.claim.value} 
                    placeholder="Enter Claim Type" />
                </div>
            </div>
    </Dialog>

    <Dialog header="User Claim" visible={state.addClaimDialog} style={{ width: '50vw' }} footer={renderFooter('add-claim-dialog')}
     onHide={() => onHide('add-claim-dialog',false)}>
            <div className='grid'>
                <div className='col-6'>
                    <h5>Key</h5>
                    <Dropdown
                        value={state.claim.value} 
                        options={claimTypes} 
                        optionValue="value"
                        optionLabel="label" 
                        onChange={e=> handleClaimTypeChange(e)}
                        placeholder="Select a User Type" />
                </div>
                <div className='col-6'>
                <h5>Value</h5>
                <InputText 
                    onChange={e=> handleClaimValueChange(e)} 
                    defaultValue={state.claim.value} 
                    placeholder="Enter Claim Type" />
                </div>
            </div>
    </Dialog>

    <Dialog header="Comfirmation" visible={state.deleteConfirmDialog} style={{ width: '50vw' }}
     footer={renderFooter('delete-claim-dialog')} onHide={() => onHide('delete-claim-dialog',false)}>
            <p>Do you wish to delete this claim ?</p>
    </Dialog>
    </>
  )
}

export default UserClaim