import { 
    HttpMethod, 
    httpFetch
} from "../../Common/Utils/HttpUtils";

interface IAuthReq {
    username: string;
    password: string;
}
  
export interface IAuthRes {
    username?: string;
    password?: string;
    message: string;
    loggedIn?: boolean;
}

const authUrl: string = 'api/authcookies';

export async function logInData(data: IAuthReq): Promise<IAuthRes> {
    const result: IAuthRes = await httpFetch<IAuthReq, IAuthRes>(
        `${authUrl}/login`, 
        HttpMethod.POST,
        data
    );

    return result;
}

export async function logOutData(): Promise<IAuthRes> {
    const result: IAuthRes = await httpFetch<IAuthReq, IAuthRes>(
        `${authUrl}/logout`, 
        HttpMethod.POST
    );

    return result;
}

export async function registerData(data: IAuthReq): Promise<IAuthRes> {
    const result: IAuthRes = await httpFetch<IAuthReq, IAuthRes>(
        `${authUrl}/register`, 
        HttpMethod.POST,
        data
    );

    return result;
}

export async function deleteUserData(data: IAuthReq): Promise<IAuthRes> {
    const result: IAuthRes = await httpFetch<IAuthReq, IAuthRes>(
        `${authUrl}/deleteuser`, 
        HttpMethod.DELETE, 
        data
    );

    return result;
}
