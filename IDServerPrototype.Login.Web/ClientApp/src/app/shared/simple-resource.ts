import { HttpClient, HttpHeaders,  } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { RequestOptionsArgs, RequestMethod, Headers, Http, Response } from '@angular/http';
import { range } from 'rxjs';
import { map } from 'rxjs/operators';

export type UrlPart = string | number;

export interface ISimpleResourceOptions {
  suppressBusy: boolean;
}

export interface ISimpleResourceRequestOptionsArgs extends RequestOptionsArgs {
  simpleResourceOptions: ISimpleResourceOptions;
}

@Injectable()
export class SimpleResource {
  url: string;
  private userContextsKey: string = "app-user-context";
  private userActiveContextsKey: string = "app-active-user-context";

  constructor(private http: Http) {
  }

  path(...urlParts: UrlPart[]): SimpleResource {
    let newResource = new SimpleResource(this.http);
    newResource.url = this.combineUrlPartsWithCurrentUrl(urlParts);
    return newResource;
  }

  private makeRequest<T>(method: RequestMethod,
    url: string = null,
    data: any = null,
    searchParams?: {},
    options: ISimpleResourceOptions = null,
    authentication: boolean = false,
    isFormDataBodyType: boolean = false
  ): Observable<T> {


    const requestArgs: ISimpleResourceRequestOptionsArgs = {
      simpleResourceOptions: options,
      method,
      headers: new Headers({
        'Content-Type': 'application/json; charset=utf-8'
      })
    };


    const requestUrl = url;


    if (data != null) {
      if (isFormDataBodyType) {
        requestArgs.body = data;
      } else {
        requestArgs.body = JSON.stringify(data);
      }
    }
    if (searchParams != null) {
      const search = new URLSearchParams();
      for (let param in searchParams) {
        if (searchParams.hasOwnProperty(param)) {
          search.set(param, searchParams[param]);
        }
      }
      if (authentication) {
        requestArgs.headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        requestArgs.body = search;
      }
      else {
        requestArgs.search = search;
      }
    } else {
      if (isFormDataBodyType) {
        requestArgs.headers = new Headers();
        requestArgs.headers.append('Accept', 'application/json');
      }
    }
    let activeUserContext: any = JSON.parse(sessionStorage.getItem(this.userActiveContextsKey));
    if (activeUserContext && activeUserContext.CurrentRole != undefined) {
      requestArgs.headers.append("current-user", activeUserContext.UserAlias);
      requestArgs.headers.append("current-user-role", activeUserContext.CurrentRole);
    }
    const response = this.http.request(requestUrl, requestArgs);
    if (!response) {
      throw `Serious failure to issue {method} to {requestUrl}, problem in a test?`;
    }

    return response.pipe(map((res: Response) => res.json()));
  }

  get<T>(url?: string, searchParams?: {}, suppressBusy: boolean = false): Observable<T> {
    return this.makeRequest<T>(RequestMethod.Get,
      url,
      null,
      searchParams,
      {
        suppressBusy
      });
  }

  post<T>(url?: string, data: any = null, searchParams?: {}, suppressBusy: boolean = false, authentication: boolean = false): Observable<T> {
    return this.makeRequest<T>(RequestMethod.Post,
      url,
      data,
      searchParams,
      {
        suppressBusy
      }, authentication);
  }

  delete<T>(url?: string, data: any = null, searchParams?: {}, suppressBusy: boolean = false, authentication: boolean = false): Observable<T> {
    return this.makeRequest(RequestMethod.Delete,
      url,
      data,
      searchParams,
      {
        suppressBusy
      }, authentication);
  }

  postFiles<T>(url?: string, data: FormData = null, searchParams?: {}, suppressBusy: boolean = false, authentication: boolean = false): Observable<T> {
    return this.makeRequest<T>(RequestMethod.Post,
      url,
      data,
      searchParams,
      {
        suppressBusy: suppressBusy
      },
      authentication,
      true
    );
  }

  put(url?: string, data: any = null, searchParams?: {}, suppressBusy: boolean = false) {
    return this.makeRequest(RequestMethod.Put,
      url,
      data,
      searchParams,
      {
        suppressBusy
      });
  }

  head(url?: string, searchParams?: {}, suppressBusy: boolean = false) {
    return this.makeRequest(RequestMethod.Head,
      url,
      null,
      searchParams,
      {
        suppressBusy
      });
  }

  private combineUrlPartsWithCurrentUrl(urlParts: UrlPart[]): string {
    if (this.url != null) {
      urlParts.unshift(this.url);
    }
    return urlParts.join('/');
  }
}
