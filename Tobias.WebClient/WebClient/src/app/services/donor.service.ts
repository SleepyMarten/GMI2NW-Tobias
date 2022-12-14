import { HttpClient, HttpHeaders, HttpParams, HttpParamsOptions } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AppConfig } from '../config/appconfig.module';

/* Application types */
import { Donor } from '../shared/donor';

@Injectable({
  providedIn: 'root'
})
export class DonorService {

  private donorUrl: string = '';

  constructor(private httpClient: HttpClient, private appConfig: AppConfig) {
    this.donorUrl = this.appConfig.ServerRootUrl + 'donor/'
  }

  public FetchDonors(): Observable<Donor[]> {
    console.log("DonorService.FetchDonors()");
    return this.httpClient.get<Donor[]>(this.donorUrl);
  }

  public FetchDonor(guid: string): Observable<Donor> {
    var httpParams: HttpParams = new HttpParams({
      fromObject: { 'guid': guid }
    });
    return this.httpClient.get<Donor>(this.donorUrl, { params: httpParams });
  }

  public PostDonor(donor: Donor): Observable<Donor> {
    var httpHeaders: HttpHeaders = new HttpHeaders();

    return this.httpClient.post<Donor>(this.donorUrl, donor);
  }

  public DeleteDonor(guid: string): Observable<string> {
    var httpParams: HttpParams = new HttpParams({
      fromObject: { 'guid': guid }
    });

    return this.httpClient.delete<string>(this.donorUrl, { params: httpParams });
  }

}
