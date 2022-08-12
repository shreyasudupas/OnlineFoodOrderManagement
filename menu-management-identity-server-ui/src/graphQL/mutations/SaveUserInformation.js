import { gql } from "@apollo/client";

export const SAVE_USERINFO = gql`
mutation ModifyUserInformation($saveUser:UserInput!){
    modifyUserInformation(userInfoInput: $saveUser)
}
`