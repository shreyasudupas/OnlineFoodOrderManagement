import './App.css';

import { Routes,Route } from 'react-router-dom';
import { AuthProvider } from '../hooks/useAuth';
import HomeLayout from './HomeLayout';
import Login from '../pages/Login';
import ProtectedLayout from './ProtectedLayout';
import UserLayout from './UserLayout';
import UserProfileOverview from '../pages/user/UserProfileOverview';
import { ListUser } from '../pages/user/ListUser';
import PageNotFound from '../pages/PageNotFound';
import Home from '../pages/Home';
import Error from '../pages/Error';
import LoginCallback from './LoginCallback';
import LogoutCallback from './LogoutCallback';
import AccessDenied from '../pages/AccessDenied';
import AccessDeniedLayout from './AccessDeniedLayout';

function App() {
  
  return (
   <AuthProvider>
      <Routes>
         <Route element={ <HomeLayout/> }>
            <Route path='/' element={ <Home/> }/>
            <Route path='login' element={ <Login/> }/>
            <Route path='signin-callback' element={ <LoginCallback/>}/>
            <Route path='signout-callback' element={ <LogoutCallback/>}/>
         </Route>

         <Route path='error' element={ <Error/> }></Route>

         <Route element={ <ProtectedLayout/> }>
            <Route path='user' element={ <UserLayout/> } >
               <Route index element={ <UserProfileOverview/> }/>
               <Route path='me' element={ <UserProfileOverview/> }/>
               <Route path='lists' element={ <ListUser/> }/>
            </Route>
            <Route path='*' element={ <PageNotFound /> }/>
         </Route>

         <Route path='access-denied' element={ <AccessDeniedLayout/> }>
         </Route>
      </Routes>
   </AuthProvider>
  );
}

export default App;
