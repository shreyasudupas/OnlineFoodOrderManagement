import React from 'react'
import { Outlet } from 'react-router-dom'

function UserOutlet() {
  console.log('User oulet called')
  return (
    <>
        <div>This is UserDashboard</div>
        <Outlet/>
    </>
    
  )
}

export default UserOutlet