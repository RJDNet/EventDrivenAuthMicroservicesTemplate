import { 
    useState,
    useEffect 
} from 'react';

import {
    IAuthRes,
    deleteUserData,
    logInData,
    logOutData,
    registerData
} from '../DataServices/AuthDataService';
import { Button } from '../../Common/Components/Button';
import { HeaderOne } from '../../Common/Components/HeaderOne';
import { Paragraph } from '../../Common/Components/Paragraph';
import { Span } from '../../Common/Components/Span';

type AuthFn = (data?: IAuthState) =>  Promise<IAuthRes>;

interface IAuthSectionProps {
    buttonsDisabled: (bool: boolean) => void;
    authButtonDisabled: boolean;
}

interface IAuthState {
    username: string;
    password: string;
    message?: string;
    loggedIn?: boolean;
}

function AuthSection(props: IAuthSectionProps): JSX.Element {
    const [userForm, setUserForm] = useState<IAuthState>({ 
        username: 'admin', 
        password: 'admin'
    });

    const [user, setUser] = useState<IAuthState>({ 
        username: '', 
        password: '', 
        message: '',
        loggedIn: false
    });

    const { 
        buttonsDisabled, 
        authButtonDisabled 
    } = props;

    useEffect(() => {
        const timeout = setTimeout(() => {
            setUser(prev => ({ 
                ...prev, 
                message: '' 
            }));
        }, 10000);

        return () => clearTimeout(timeout);
    }, [user.message]);  
    
    function deleteUser(data: IAuthState): void {
        deleteUserData(data);
        setUser((prev) => ({ 
            ...prev, 
            loggedIn: false 
        }));
        buttonsDisabled(true);
    }

    async function authFunctions(fn: AuthFn, data?: IAuthState): Promise<void> {
        const result: IAuthRes = await fn(data);
        setUser((prev) => ({ ...prev, ...result }));
        buttonsDisabled(true);
    }

    return (
        <>
            <HeaderOne>Auth</HeaderOne>
            <Paragraph>Must populate username/password fields.</Paragraph>
            <Span>Username: </Span>
                <input
                    name='username'
                    type='text'
                    value={userForm.username}
                    onChange={(e) => setUserForm(prev => ({ ...prev, username: e.target.value }))}
                 />
            <Span> Password: </Span>
                <input
                    name='password'
                    type='text'
                    value={userForm.password}
                    onChange={(e) => setUserForm(prev => ({ ...prev, password: e.target.value }))}
                />
            <h4>Logged In: {user.loggedIn ? `You are Logged In as: ${user.username}` : 'Logged Out'}</h4>
            <h2>Auth Message: {user.message}</h2>
            <br/>
            <Button 
                disable={user.loggedIn || authButtonDisabled}
                onClick={() => authFunctions(logInData, userForm)}
            >
                Log In
            </Button>
            <Button 
                disable={!user.loggedIn || authButtonDisabled}
                onClick={() => authFunctions(logOutData)}
            >
                Log Out
            </Button>
            <br/>
            <Button 
                disable={user.loggedIn || authButtonDisabled}
                onClick={() => authFunctions(registerData, userForm)}
            >
                Register
            </Button>
            <Button 
                disable={!user.loggedIn || authButtonDisabled}
                onClick={() => deleteUser(userForm)}
            >
                Delete User
            </Button>
        </>
    );
}

export default AuthSection;