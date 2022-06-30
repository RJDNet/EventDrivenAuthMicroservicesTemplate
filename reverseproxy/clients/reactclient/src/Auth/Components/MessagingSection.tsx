import { Button } from "../../Common/Components/Button";
import { HeaderOne } from "../../Common/Components/HeaderOne";
import { sendMessageData, sendRpcMessageData } from "../DataServices/MessagingTestDataService";

interface IMessagingSectionProps {
    buttonsDisabled: (bool: boolean) => void;
    authButtonDisabled: boolean;
}

function MessagingSection(props: IMessagingSectionProps): JSX.Element {
    const { 
        buttonsDisabled,
        authButtonDisabled
     } = props;

    function sendRpcMessage(): void {
        sendRpcMessageData();
        buttonsDisabled(true);
    }
    
    function sendMessage(): void {
        sendMessageData();
        buttonsDisabled(true);
    }  

    return (
        <>
        <HeaderOne>Messaging</HeaderOne>
            <Button
                disable={authButtonDisabled}
                onClick={() => sendRpcMessage()}
            >
                Rpc Message Tester
            </Button>
            <Button
                disable={authButtonDisabled}
                onClick={() => sendMessage()}
            >
                Message Tester
            </Button>
        </>
    );
}

export default MessagingSection;