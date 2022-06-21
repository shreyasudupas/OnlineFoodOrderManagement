import React from 'react'
import { Outlet } from 'react-router-dom'

function UserLayout() {
  console.log('User Layout called')
  return (
    <>
        <div>This is User Layout</div>
        <Outlet/>
    </>
    
  )
}

export default UserLayout