import React from 'react'
import { useAuth } from '../../hooks/useAuth'

function UserProfileOverview() {
    const getUserContext = useAuth()
    console.log('User overview called')
  return (
    <div>Hello {getUserContext.user}</div>
  )
}

export default UserProfileOverview