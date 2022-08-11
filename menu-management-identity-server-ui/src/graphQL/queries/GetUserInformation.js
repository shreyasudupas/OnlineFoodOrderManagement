import { gql } from "@apollo/client";

export const GET_USERINFORMATION = gql`
query GetUserInformation($userId:String!){
  userInformation (userId:$userId){
    id
    userName
    email
    cartAmount
    points
    claims{
      label
      value
    }
    address {
      id
      fullAddress
      city
      area
      state
      stateId
      myStates {
        label
        value
      }
      isActive
      city
      myCities {
        label
        value
      }
      cityId
      area
      myAreas {
        label
        value
      }
      areaId
    }
  }
}
`;