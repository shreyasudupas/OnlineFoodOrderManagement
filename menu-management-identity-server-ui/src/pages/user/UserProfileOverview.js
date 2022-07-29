import React, { useState } from 'react'
import { useAuth } from '../../hooks/useAuth'
import { Card } from 'primereact/card'
import { InputText } from 'primereact/inputtext';
import { InputNumber } from 'primereact/inputnumber';
import { Checkbox } from 'primereact/checkbox';
import '../../components/App.css'
import { Button } from 'primereact/button';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { InputTextarea } from 'primereact/inputtextarea';
import { Dropdown } from 'primereact/dropdown';
import UserClaim from '../../components/user/UserClaim';
import ImageUpload from '../../components/ImageUpload';

function UserProfileOverview() {
    const getUserContext = useAuth()
    const [checked,setChecked] = useState(false)

    const claimList = [
      {'key':'email','value':'xyz@cmm.com'},
      {'key':'role','value':'admin'},
      {'key':'username','value':'admin'}
    ]

    const cities  = [
      {label: 'New York', value: 'NY'},
      {label: 'Rome', value: 'RM'},
      {label: 'London', value: 'LDN'},
      {label: 'Istanbul', value: 'IST'},
      {label: 'Paris', value: 'PRS'}
  ];

    console.log('User overview called')
  return (
    <Card title="Welcome to the Content Management Dashboard" subTitle="User Profile">
      <div className='grid p-fluid'>
        <div className='col-5'>

        </div>
        <div className='col-7'>
          <div className='col-12'>
            <h5>Username</h5>
              <div className="p-inputgroup">
                <span className="p-inputgroup-addon">
                  <i className="pi pi-user"></i>
                </span>
                <InputText placeholder="Username" />
              </div>
          </div>
          <div className='col-12'>
            <h5>Email</h5>
            <div className="p-inputgroup">
                <span className="p-inputgroup-addon">www</span>
                <InputText placeholder="Email" />
            </div>
          </div>
          <div className='col-12'>
          <h5>Cart Ammount</h5>
          <div className="p-inputgroup">
            <span className="p-inputgroup-addon">$</span>
            <InputNumber  placeholder="CartAmmount" />
          </div>
        </div>
        <div className='col-12'>
          <h5>Points</h5>
          <div className="p-inputgroup">
            <span className="p-inputgroup-addon">$</span>
            <InputNumber placeholder="Points" />
          </div>
        </div>
        <div className='col-12'>
          <div className="field-checkbox">
              <Checkbox inputId="binary" onChange={e => setChecked(e.checked)} checked={checked}></Checkbox>
              <label htmlFor="binary">Active</label>
            </div>
        </div>
        </div>
        <div className='p-col-4 col-offset-5'>
          <Button label="Save" className="p-button-rounded" />
        </div>
      </div>

      <div className='grid p-fluid'>
        <div className='p-col-12 w-full'>
          <ImageUpload/>
        </div>
      </div>
         
        <div className="grid p-fluid App-Logo">
          <UserClaim userClaims={claimList}/>
        </div>

        <div className='grid p-fluid'>
        <div className='col-12'>
          <h5>User Address</h5>
        </div>
        <div className='col-12'>
        <Accordion activeIndex={0}>
            <AccordionTab header="Address I">
              <div className='grid'>
                <div className='col-12'>
                  <h5>Full Address</h5>
                  <InputTextarea className='w-full' rows={5} cols={30}  />

                  <div className="field-checkbox">
                    <Checkbox inputId="acticeAddress" onChange={e => setChecked(e.checked)} checked={checked}></Checkbox>
                    <label htmlFor="acticeAddress">IsActive</label>
                  </div>

                  <h5>State</h5>
                  <Dropdown  options={cities} />

                  <h5>City</h5>
                  <Dropdown  options={cities} />

                  <h5>Area</h5>
                  <Dropdown  options={cities} />

                  <Button label="Save" />
                </div>
                
              </div>
            </AccordionTab>
            <AccordionTab header="Address II">
                <p>Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi
                architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione
                voluptatem sequi nesciunt. Consectetur, adipisci velit, sed quia non numquam eius modi.</p>
            </AccordionTab>
            <AccordionTab header="Address III">
              <p>At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati
              cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio.
              Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus.</p>
            </AccordionTab>
          </Accordion>  
        </div>
                
        </div>
    </Card>
  )
}

export default UserProfileOverview