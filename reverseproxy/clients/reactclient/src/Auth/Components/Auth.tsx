import { 
    useEffect, 
    useState 
} from 'react';

import { getCsrfToken } from '../DataServices/CsrfTokenDataService';
import {
    deleteUserData,
    IAuthTesterRes,
    IAuthLoginRes, 
    IAuthRegisterRes, 
    logInData, 
    logOutData, 
    registerData, 
    testerData 
} from '../DataServices/AuthDataService';
import { Button } from '../../Common/Components/Button';

interface IAuthState {
    username: string;
    password: string;
    message?: string;
}

function Auth(): JSX.Element {
    const [userForm, setUserForm] = useState<IAuthState>({ 
        username: '', 
        password: '',
    });

    const [user, setUser] = useState<IAuthState>({ 
        username: '', 
        password: '', 
        message: ''
    });

    useEffect(() => {
        const timeout = setTimeout(() => {
            setUser(prev => ({ ...prev, message: ''}));
        }, 10000);

        return () => clearTimeout(timeout);
    }, [user.message]);

    async function tester(data: IAuthState): Promise<void> {
        const result: IAuthTesterRes = await testerData(data);
        setUser({ ...result });
    }

    async function logIn(data: IAuthState): Promise<void> {
        const result: IAuthLoginRes = await logInData(data);
        setUser({ ...result });
    }
    
    function logOut(): void {
        logOutData();
    }

    async function register(data: IAuthState): Promise<void> {
        const result: IAuthRegisterRes = await registerData(data);
        setUser({ ...result });
    }
    
    function deleteUser(data: IAuthState): void {
        deleteUserData(data);
    }

    return (
        <div>
            <Button onClick={() => getCsrfToken()}>Get Csrf Token</Button>
            <Button onClick={() => tester(userForm)}>Tester</Button>
            <Button onClick={() => logIn(userForm)}>Log In</Button>
            <Button onClick={() => logOut()}>Log Out</Button>
            <Button onClick={() => register(userForm)}>Register</Button>
            <Button onClick={() => deleteUser(userForm)}>Delete User</Button>
            <div>
                Username: 
                <input 
                    name='username' 
                    type='text' 
                    value={userForm.username} 
                    onChange={(e) => setUserForm(prev => ({ ...prev, username: e.target.value }))} 
                 />
            </div>
            <div>
                Password: 
                <input 
                    name='password' 
                    type='text' 
                    value={userForm.password} 
                    onChange={(e) => setUserForm(prev => ({ ...prev, password: e.target.value }))} 
                />
            </div>
            <div>Username: {user.username}</div>
            <div>Password: {user.password}</div>
            <h2>Message: {user.message}</h2>
        </div>
    );
}

export default Auth;
