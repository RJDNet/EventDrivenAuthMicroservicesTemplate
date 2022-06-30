import { 
    useState,
    useCallback
} from 'react';

import CsrfSection from './CsrfSection';
import MessagingSection from './MessagingSection';
import AuthSection from './AuthSection';

function Auth(): JSX.Element {
    const [authButtonDisabled, setAuthButtonsDisabled] = useState<boolean>(true); 

    const buttonsDisabled = useCallback((bool: boolean) => {
        setAuthButtonsDisabled(bool);
    }, []);
    
    return (
        <>
            <CsrfSection buttonsDisabled={buttonsDisabled} />
            <MessagingSection authButtonDisabled={authButtonDisabled} buttonsDisabled={buttonsDisabled} />
            <AuthSection authButtonDisabled={authButtonDisabled} buttonsDisabled={buttonsDisabled} />
        </>
    );
}

export default Auth;
