import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvestigationResultsComponent } from './investigation-results.component';

describe('InvestigationResultsComponent', () => {
  let component: InvestigationResultsComponent;
  let fixture: ComponentFixture<InvestigationResultsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvestigationResultsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvestigationResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
