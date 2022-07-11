import React, { useEffect } from 'react'
import { Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import { InputText } from 'primereact/inputtext';
import { Menubar } from 'primereact/menubar';
import './App.css'

function ProtectedLayout() {
    const auth = useAuth()
    const navigate = useNavigate()
    let isAdmin = 'Loading'

    const items = [
       {
          label:'Edit',
          icon:'pi pi-fw pi-pencil',
          items:[
             {
                label:'Left',
                icon:'pi pi-fw pi-align-left'
             },
             {
                label:'Right',
                icon:'pi pi-fw pi-align-right'
             },
             {
                label:'Center',
                icon:'pi pi-fw pi-align-center'
             },
             {
                label:'Justify',
                icon:'pi pi-fw pi-align-justify'
             },
     
          ]
       },
       {
          label:'Users',
          icon:'pi pi-fw pi-user',
          items:[
              {
                label:'Dashboard',
                icon: 'pi pi-fw pi-id-card',
                command: ()=>{ navigate('user/me') }
              },
             {
                label:'New',
                icon:'pi pi-fw pi-user-plus',
     
             },
             {
                label:'Delete',
                icon:'pi pi-fw pi-user-minus',
     
             },
             {
                label:'List',
                icon:'pi pi-fw pi-list',
                command: ()=>{ navigate('user/lists') }
     
             }
          ]
       },
       {
        label:'Logout',
        icon:'pi pi-fw pi-power-off',
        command: () => { handleLogout() }
        }
     ];

   if(!auth.isAuthenticated()){
      auth.signinRedirect()
   }else{
      //call the isAdminRole
      isAdmin = auth.userRoleIsAdmin()
   }

   //This will give the result once promise will finish giving its result
   useEffect(()=>{
      async function checkIfAdmin(){
         let role = await auth.userRoleIsAdmin()

         if(role!=="admin"){
            navigate('/access-denied')
         }
      }
      checkIfAdmin()
   },[isAdmin])

    const handleLogout = () => {
      auth.logout()
    }

    const start = <img alt="logo" src="assets/menu/MenuLogo1.png" onError={(e) => e.target.src='https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png'} 
     height="40" className="mr-2"></img>;
    const end = <InputText placeholder="Search" type="text" />;

  return (
    <>
        <Menubar model={items} start={start} end={end} />
        <div className='App-Logo'>
            <div className='App-login'>
                <Outlet/>
            </div>
        </div>
    </>
  )
}

export default ProtectedLayout