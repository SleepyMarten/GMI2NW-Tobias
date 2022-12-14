import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AppConfig } from '../config/appconfig.module';

/* Application types */
import { CompatibilityResult } from '../shared/compatibilityresult';

@Injectable({
  providedIn: 'root'
})
export class CompatibilityService {

  private compatibilityUrl: string = '';

  constructor(private httpClient: HttpClient, private appConfig: AppConfig) {
    this.compatibilityUrl = this.appConfig.ServerRootUrl + 'Compatibility/'
  }

  public FetchCompatibilityResult(donorGuid: string, recipientGuid: string): Observable<CompatibilityResult> {
    console.log("CompatibilityService.FetchCompatibilityResult()");
    var httpParams: HttpParams = new HttpParams({ fromObject: {'donorGuid' : donorGuid, 'recipientGuid' : recipientGuid}   })

    return this.httpClient.get<CompatibilityResult>(this.compatibilityUrl, { params: httpParams });
  }
}
