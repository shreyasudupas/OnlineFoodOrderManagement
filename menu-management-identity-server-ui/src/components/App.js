import './App.css';

import { Routes,Route } from 'react-router-dom';
import { AuthProvider } from '../hooks/useAuth';
import HomeLayout from './HomeLayout';
import Login from '../pages/Login';
import ProtectedLayout from './ProtectedLayout';
import UserLayout from './user/UserLayout';
import UserProfileOverview from './user/UserProfileOverview';
import { ListUser } from './user/ListUser';
import PageNotFound from '../pages/PageNotFound';
import Home from '../pages/Home';

function App() {
  
  return (
   <AuthProvider>
      <Routes>
         <Route element={ <HomeLayout/> }>
            <Route path='/' element={ <Home/> }/>
            <Route path='login' element={ <Login/> }/>
         </Route>

         <Route element={ <ProtectedLayout/> }>
            <Route path='user' element={ <UserLayout/> } >
               <Route index element={ <UserProfileOverview/> }/>
               <Route path='me' element={ <UserProfileOverview/> }/>
               <Route path='lists' element={ <ListUser/> }/>
            </Route>
            <Route path='*' element={ <PageNotFound /> }/>
         </Route>
         
      </Routes>
   </AuthProvider>
  );
}

export default App;
