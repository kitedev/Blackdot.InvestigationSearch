import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvestigationSearchComponent } from './investigation-search.component';

describe('InvestigationSearchComponent', () => {
  let component: InvestigationSearchComponent;
  let fixture: ComponentFixture<InvestigationSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvestigationSearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvestigationSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
