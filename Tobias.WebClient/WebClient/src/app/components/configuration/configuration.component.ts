import { Component, OnInit } from '@angular/core';
import { AppConfig } from 'src/app/config/appconfig.module';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css']
})
export class ConfigurationComponent implements OnInit {

  constructor(public appConfig: AppConfig) { }

  ngOnInit(): void {
  }

}
