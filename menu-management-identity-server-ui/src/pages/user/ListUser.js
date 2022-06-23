import React from 'react'
import { useAuth } from '../../hooks/useAuth'

export const ListUser = () => {
  const { user } = useAuth()

  console.log('List User called')

  return (
    <div> {user} - List of Users</div>
  )
}
