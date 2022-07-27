import React, { useState } from 'react'
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { InputText } from 'primereact/inputtext';

function UserClaim({userClaims}) {
    //console.log('User claim fun component called')
    const [displayEditClaimDialog, setDisplayEditClaimDialog] = useState(false);
    const [displayAddClaimDialog, setDisplayAddClaimDialog] = useState(false);
    const [displayClaimConfirm, setDisplayClaimConfirm] = useState(false);
    const [claims,setClaims] = useState(userClaims)
    const [userClaim,setUserClaim] = useState({
        'key':null,
        'value':null
    })

    const claimTypes = [
        { name: 'username', code: 'username' },
        { name: 'email', code: 'email' },
        { name: 'role', code: 'role' },
        { name: 'picture', code: 'picture' }
    ];

    const dialogFuncMap = {
        'displayEditClaimDialog': setDisplayEditClaimDialog,
        'displayAddClaimDialog': setDisplayAddClaimDialog,
        'displayClaimDeleteConfirmation': setDisplayClaimConfirm
    }

    const onClick = (name) => {
        dialogFuncMap[`${name}`](true);
    }

    const onHide = (name,save) => {
        dialogFuncMap[`${name}`](false);

        //delete operation
        if(name==="displayClaimDeleteConfirmation" && save === true){
            userClaims = userClaims.filter(item=> item.key!= userClaim["key"])
            //console.log(userClaims)
            setClaims(userClaims)
        }

        //edit operation
        if(name==="displayEditClaimDialog" && save === true){
            setClaims(
                claims.map((claim)=>
                claim.key === userClaim["key"]? {...claim,value:userClaim["value"]} : {...claim}
                )
            );
        }

        //add operation
        if(name==="displayAddClaimDialog" && save === true){
           setClaims((prevClaims)=>[...prevClaims,{
            key:userClaim["key"],
            value:userClaim["value"]
           }])
           //console.log(userClaims)
        }
        

        //remove the individual user claim
        setUserClaim({'key':null,'value':null})
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
        setUserClaim(rowData)
        //console.log(userClaim)
        onClick('displayEditClaimDialog')
    }

    //Delete Opertaions
    const oprationDeleteTemplate = (rowData) => {
        return <Button icon="pi pi-user-minus" onClick={() => deleteClaim(rowData)}/>
    }

    const deleteClaim = (rowData) => {
        //console.log(rowData)
        setUserClaim(rowData)
        onClick('displayClaimDeleteConfirmation')
    }

    //Dialog change of UserClaim for add/Edit
    const handleClaimValueChange = (event) =>{
        setUserClaim({ ...userClaim, value:event.target.value })
    }
    const handleClaimTypeChange = (event) =>{
        setUserClaim({ ...userClaim, key:event.target.value })
    }

    //called when add is clicked
    const addPreUserClaim = () =>{
        onClick('displayAddClaimDialog')
    }

  return (
    <>
    <div className='col-12'>
        <DataTable header="User Claims" value={claims} responsiveLayout="scroll" className='w-full'>
            <Column field="key" header="Claim Type"></Column>
            <Column field="value" header="Claim Name"></Column>
            <Column field='edit' header="Edit" body={oprationAddTemplate}></Column>
            <Column field='delete' header="Delete" body={oprationDeleteTemplate}></Column>
        </DataTable>
    </div>
    <div className='col-6'>
        <Button label="Add" className="p-button-info" onClick={() => addPreUserClaim()}/>
    </div>
    {/* <div className='col-6'>
        <Button label="Delete" className="p-button-danger" />
    </div> */}
    <Dialog header="User Claim" visible={displayEditClaimDialog} style={{ width: '50vw' }} footer={renderFooter('displayEditClaimDialog')}
     onHide={() => onHide('displayEditClaimDialog',false)}>
            <div className='grid'>
                <div className='col-6'>
                    <h5>Key</h5>
                    <Dropdown
                        value={userClaim.key} 
                        options={claimTypes} 
                        optionValue="code"
                        optionLabel="name" 
                        onChange={e=> handleClaimTypeChange(e)}
                        placeholder="Select a User Type" />
                </div>
                <div className='col-6'>
                <h5>Value</h5>
                <InputText 
                    onChange={e=> handleClaimValueChange(e)} 
                    defaultValue={userClaim.value} 
                    placeholder="Enter Claim Type" />
                </div>
            </div>
    </Dialog>

    <Dialog header="User Claim" visible={displayAddClaimDialog} style={{ width: '50vw' }} footer={renderFooter('displayAddClaimDialog')}
     onHide={() => onHide('displayAddClaimDialog',false)}>
            <div className='grid'>
                <div className='col-6'>
                    <h5>Key</h5>
                    <Dropdown
                        value={userClaim.key} 
                        options={claimTypes} 
                        optionValue="code"
                        optionLabel="name" 
                        onChange={e=> handleClaimTypeChange(e)}
                        placeholder="Select a User Type" />
                </div>
                <div className='col-6'>
                <h5>Value</h5>
                <InputText 
                    onChange={e=> handleClaimValueChange(e)} 
                    defaultValue={userClaim.value} 
                    placeholder="Enter Claim Type" />
                </div>
            </div>
    </Dialog>

    <Dialog header="Comfirmation" visible={displayClaimConfirm} style={{ width: '50vw' }}
     footer={renderFooter('displayClaimDeleteConfirmation')} onHide={() => onHide('displayClaimDeleteConfirmation',false)}>
            <p>Do you wish to delete this claim ?</p>
    </Dialog>
        
    </>
  )
}

export default UserClaim