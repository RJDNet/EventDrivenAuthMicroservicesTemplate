import { 
    HttpMethod, 
    httpFetchNoResData
} from './../../Common/Utils/HttpUtils';

const messagingTestUrl: string = 'api/messagingtest';

export async function sendRpcMessageData(): Promise<void> {
    await httpFetchNoResData(`${messagingTestUrl}/rpcmessagetest`, HttpMethod.POST);
}

export async function sendMessageData(): Promise<void> {
    await httpFetchNoResData(`${messagingTestUrl}/messagetest`, HttpMethod.POST);
}