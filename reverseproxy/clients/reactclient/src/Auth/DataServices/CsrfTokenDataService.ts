import { 
    HttpMethod, 
    httpFetchNoResData
} from './../../Common/Utils/HttpUtils';

const csrfUrl: string = 'api/csrftoken';

export async function getCsrfToken(): Promise<void> {
    await httpFetchNoResData(`${csrfUrl}/getcsrftoken`, HttpMethod.GET);
}