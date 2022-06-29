import { 
    HttpMethod, 
    httpFetch,
    httpFetchNoResData
} from "../../Common/Utils/HttpUtils";

interface IAuthReq {
    username: string;
    password: string;
}
  
export interface IAuthTesterRes {
    username: string;
    password: string;
}

export interface IAuthLoginRes extends IAuthTesterRes {
    message?: string;
}

export interface IAuthRegisterRes extends IAuthTesterRes {
    message?: string;
}

export interface IAuthDeleteUserRes extends IAuthTesterRes {}

const authUrl: string = 'api/authcookies';

export async function testerData(data: IAuthReq): Promise<IAuthTesterRes> {
    const result: IAuthTesterRes = await httpFetch<IAuthReq, IAuthTesterRes>(
        `${authUrl}/authtester`, 
        HttpMethod.POST,
        data
    );

    return result;
}

export async function logInData(data: IAuthReq): Promise<IAuthLoginRes> {
    const result: IAuthLoginRes = await httpFetch<IAuthReq, IAuthLoginRes>(
        `${authUrl}/login`, 
        HttpMethod.POST,
        data
    );

    return result;
}

export function logOutData(): void {
    httpFetchNoResData(`${authUrl}/logout`, HttpMethod.POST);
}

export async function registerData(data: IAuthReq): Promise<IAuthRegisterRes> {
    const result: IAuthRegisterRes = await httpFetch<IAuthReq, IAuthRegisterRes>(
        `${authUrl}/register`, 
        HttpMethod.POST,
        data
    );

    return result;
}

export function deleteUserData(data: IAuthReq): void {
    httpFetchNoResData<IAuthDeleteUserRes>(
        `${authUrl}/deleteuser`, 
        HttpMethod.DELETE, 
        data
    );
}
