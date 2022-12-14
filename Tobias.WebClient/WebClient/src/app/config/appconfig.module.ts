import { Injectable, NgModule } from "@angular/core";

@Injectable({
    providedIn: 'root'
  })
  @NgModule({})
  export class AppConfig {
    public readonly ServerProtocol : string = 'https';
    //public readonly ServerProtocol : string = 'http';
    public readonly ServerName : string= 'localhost';
    public readonly ServerPort : string= '44357';
    //public readonly ServerPort : string= '38660';
    public readonly ServerRootUrl : string= '';
    
    constructor ()
    {
      this.ServerRootUrl = this.ServerProtocol + '://' + 
                           this.ServerName + ':' + 
                           this.ServerPort + '/';
    }
  }  
