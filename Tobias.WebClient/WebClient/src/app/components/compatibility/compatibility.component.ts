import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

/* Application-specific types */
import { Donor } from '../../shared/donor';
import { DonorService } from '../../services/donor.service';
import { Recipient } from '../../shared/recipient';
import { RecipientService } from '../../services/recipient.service';
import { CompatibilityService } from '../../services/compatibility.service';
import { PageStates } from 'src/app/shared/ItemStates';
import { CompatibilityStates } from 'src/app/shared/CompatibilityStates';
import { CompatibilityResult } from 'src/app/shared/compatibilityresult';


@Component({
  selector: 'app-compatibility',
  templateUrl: './compatibility.component.html',
  styleUrls: ['./compatibility.component.css']
})
export class CompatibilityComponent implements OnInit {

  //Aliases for enums, required for html to be able to access the enum values
  public PageStates = PageStates;
  public CompatibilityStates = CompatibilityStates;

  //Properties
  public pageStateDonors: PageStates = PageStates.Uninitialized;
  public pageStateRecipients: PageStates = PageStates.Uninitialized;
  public pageStateCompatibilityResult: PageStates = PageStates.Uninitialized;

  //Items of this page
  public donors: Donor[] = [];
  public selectedDonorGuid: string = "";
  public recipients: Recipient[] = [];
  public selectedRecipientGuid: string = "";

  public compatibilityResult: CompatibilityResult = new CompatibilityResult;

  constructor(public donorService: DonorService, public recipientService: RecipientService, public compatibilityService: CompatibilityService, private router: Router, private activatedRoute: ActivatedRoute) {

  }

  private FetchDonors(): void {
    console.log("CompatibilityComponent.FetchDonors()");

    this.pageStateDonors = PageStates.Fetching;
    this.donors = [];

    this.donorService.FetchDonors().subscribe(donors => {
      this.donors = donors;
      this.pageStateDonors = PageStates.FetchedOrNew;
    });
  }

  private FetchRecipients(): void {

    console.log("CompatibilityComponent.FetchRecipients()");

    this.pageStateRecipients = PageStates.Fetching;
    this.recipients = [];

    this.recipientService.FetchRecipients().subscribe(recipients => {
      this.recipients = recipients;
      this.pageStateRecipients = PageStates.FetchedOrNew;
    });
  }

  public ActionSelectDonor(donorGuid: string): void {
    this.selectedDonorGuid = donorGuid;
  }

  public ActionSelectRecipient(recipientGuid: string): void {
    this.selectedRecipientGuid = recipientGuid;
  }

  public ActionFetchCompatibilityResult(): void {
    console.log("CompatibilityComponent.ActionFetchCompatibilityResult()");

    this.pageStateCompatibilityResult = PageStates.Fetching;
    this.compatibilityService.FetchCompatibilityResult(this.selectedDonorGuid, this.selectedRecipientGuid).subscribe(compatibilityResult => {
      console.log("CompatibilityService.FetchCompatibilityResult() returned: " + JSON.stringify(compatibilityResult));
      this.compatibilityResult = compatibilityResult;
      this.pageStateCompatibilityResult = PageStates.FetchedOrNew;
    });
  }

  ngOnInit(): void {

    console.log("CompatibilityComponent.ngOnInit()")

    this.FetchDonors();
    this.FetchRecipients();
  }

}