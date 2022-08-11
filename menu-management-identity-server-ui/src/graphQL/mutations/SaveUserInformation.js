import { gql } from "@apollo/client";

export const SAVE_USERINFO = gql`
mutation saveUser($saveUser:UserInput){
    modifyUserInformation(userInfoInput: $saveUser)
}
`