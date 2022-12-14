import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipientlistComponent } from './recipientlist.component';

describe('RecipientlistComponent', () => {
  let component: RecipientlistComponent;
  let fixture: ComponentFixture<RecipientlistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecipientlistComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecipientlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
