import './App.css';

import Login from './components/Login';
import { Routes,Route } from 'react-router-dom';
import Home from './components/Home';
import PageNotFound from './components/PageNotFound';
import { ListUser } from './components/user/ListUser';
import UserOutlet from './components/user/UserOutlet';
import UserProfileOverview from './components/user/UserProfileOverview';
import { AuthProvider } from './components/utilities/auth';
import NavMenuBar from './components/NavMenuBar';
import { RequireAuth } from './components/utilities/RequireAuth';

function App() {
  
  return (
   <AuthProvider>
      <NavMenuBar/>
      <div className='App-Logo'>
         <div className='App-login'>
            <Routes>
               <Route path='/' element={<Home/>}/>
               <Route path='login' element={<Login/>}/>
               <Route path='user' element={<RequireAuth><UserOutlet/></RequireAuth>}>
                  {/* default route under user usign index */}
                  <Route index element={<UserProfileOverview/>}/>
                  <Route path='mine' element={<UserProfileOverview/>}/>
                  <Route path='list' element={<ListUser/>}/>
               </Route>
               <Route path='*' element={<PageNotFound/>}/>
            </Routes>
         </div>
      </div>
   </AuthProvider>
  );
}

export default App;
