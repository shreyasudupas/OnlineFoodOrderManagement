import React from 'react'
import { Menubar } from 'primereact/menubar';
import { useAuth } from './utilities/auth';
import { InputText } from 'primereact/inputtext';
import { useNavigate } from 'react-router-dom';

function NavMenuBar() {
    const auth = useAuth()
    const navigate = useNavigate()

    let items = []

    //if user login for first time show only login nav
    if(!auth.user){
        items = [
            {
                label:'Login',
                icon:'pi pi-fw pi-unlock',
                command:()=>{window.location="/login"}
             }
        ]
    }
    else{
        items = [
            {
               label:'Home',
               icon:'pi pi-home',
               command:()=>{window.location = "/"}
         
            },
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
                    command: ()=>{ window.location = '/user/mine' }
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
                    command: ()=>{ window.location = '/user/list' }
         
                 }
              ]
           },
           {
            label:'Logout',
            icon:'pi pi-fw pi-power-off',
            command: () => { handleLogout() }
            }
         ];
    }

    const handleLogout = () => {
        auth.logout()
        navigate('/')
    }
    
     
     const start = <img alt="logo" src="assets/menu/MenuLogo1.png" onError={(e) => e.target.src='https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png'} 
     height="40" className="mr-2"></img>;
     const end = <InputText placeholder="Search" type="text" />;

  return (
    <>
        <Menubar model={items} start={start} end={end} />
    </>
  )
}

export default NavMenuBar