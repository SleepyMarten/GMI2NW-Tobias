import { HttpClient, HttpHeaders, HttpParams, HttpParamsOptions } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AppConfig } from '../config/appconfig.module';

/* Application types */
import { Recipient } from '../shared/recipient';

@Injectable({
  providedIn: 'root'
})
export class RecipientService {

  private recipientUrl: string = '';

  constructor(private httpClient: HttpClient, private appConfig: AppConfig) {
    this.recipientUrl = this.appConfig.ServerRootUrl + 'recipient/'
  }

  public FetchRecipients(): Observable<Recipient[]> {
    console.log("RecipientService.FetchRecipients()");
    return this.httpClient.get<Recipient[]>(this.recipientUrl);
  }

  public FetchRecipient(guid: string): Observable<Recipient> {
    var httpParams: HttpParams = new HttpParams({
      fromObject: { 'guid': guid }
    });
    return this.httpClient.get<Recipient>(this.recipientUrl, { params: httpParams });
  }

  public PostRecipient(recipient: Recipient): Observable<Recipient> {
    var httpHeaders: HttpHeaders = new HttpHeaders();
    httpHeaders.set("Content-Type", "application/json;charset=utf-8");
    httpHeaders.set("Host", "localhost");

    return this.httpClient.post<Recipient>(this.recipientUrl, recipient);
  }

  public DeleteRecipient(guid: string): Observable<string> {
    var httpParams: HttpParams = new HttpParams({
      fromObject: { 'guid': guid }
    });
    return this.httpClient.delete<string>(this.recipientUrl, { params: httpParams });
  }

}
