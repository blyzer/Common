import { TestBed, inject } from '@angular/core/testing';

import { PoliticalDivisionService } from './political-division.service';

describe('PoliticalDivisionService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PoliticalDivisionService]
    });
  });

  it('should be created', inject([PoliticalDivisionService], (service: PoliticalDivisionService) => {
    expect(service).toBeTruthy();
  }));
});
