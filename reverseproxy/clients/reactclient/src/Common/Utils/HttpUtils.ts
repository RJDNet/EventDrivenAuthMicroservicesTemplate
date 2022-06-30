export enum HttpMethod {
    GET = 'GET',
    POST = 'POST',
    PUT = 'PUT',
    DELETE = 'DELETE'
}

const host: string = process.env.NODE_ENV === 'development' ? 'http://localhost:5000/' : 'http://localhost:3000/';

export async function httpFetchNoResData<Res>(url: string, method: HttpMethod, data?: Res): Promise<void> {
  const headers: RequestInit = setupHeaders(method, data);

  try {
    await fetch(`${host}${url}`, headers);
  }
  catch(e) {
    console.log(e);
    throw e;
  }
}

export async function httpFetch<Req, Res>(url: string, method: HttpMethod, data?: Req): Promise<Res> {      
  const headers: RequestInit = setupHeaders<Req>(method, data);

  try {
      const fetchData: Response = await fetch(`${host}${url}`, headers);
      const jsonData: Promise<Res> = await fetchData.json();
      
      if(fetchData.status === 200) {
        return jsonData;
      }

      // eslint-disable-next-line no-throw-literal
      throw `Error Response: ${JSON.stringify(jsonData)}`;
  }
  catch(e) {
    throw e;
  }
}

function setupHeaders<Req>(method: HttpMethod, data?: Req): RequestInit {
  const headers: RequestInit = {
    method: method, // *GET, POST, PUT, DELETE, etc.
    headers: {
      'Content-Type': 'application/json',
      // 'Content-Type': 'application/x-www-form-urlencoded',
      'X-XSRF-TOKEN': getCsrfTokenFromCookie("XSRF-TOKEN"),
      'X-Forwarded-Host': host
    },
    body: JSON.stringify(data), // body data type must match "Content-
    mode: 'cors', // no-cors, *cors, same-origin
    //cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
    credentials: 'include', // include, *same-origin, omit
    //redirect: 'follow', // manual, *follow, error
    //referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
  }

  return headers;
}

function getCsrfTokenFromCookie(cname: string): string {
  const name: string = cname + '=';
  const decodedCookie: string = decodeURIComponent(document.cookie);
  const ca: string[] = decodedCookie.split(';');
  
  for (let i: number = 0; i < ca.length; i++) {
    let c: string = ca[i];

    while (c.charAt(0) === ' ') {
      c = c.substring(1);
    }

    if (c.indexOf(name) === 0) {
      return c.substring(name.length, c.length);
    }
  }

  return '';
}
