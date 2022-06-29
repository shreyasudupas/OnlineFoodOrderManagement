import React from 'react'
import { Navigate, Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import { InputText } from 'primereact/inputtext';
import { Menubar } from 'primereact/menubar';
import './App.css';

function HomeLayout() {
    const auth = useAuth()
    const navigate = useNavigate()

    const items = [
        {
           label:'Home',
           icon:'pi pi-home',
           command:()=>{ navigate('') }
     
        },
        {
           label:'Login',
           icon:'pi pi-fw pi-unlock',
           command:()=>{ //navigate('login')
            auth.signinRedirect() 
          }
        }
   ]

    // if(auth.user){
    //     return <Navigate to="user" />
    // }
    if(auth.isAuthenticated()){
      return <Navigate to="user" />
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

export default HomeLayout