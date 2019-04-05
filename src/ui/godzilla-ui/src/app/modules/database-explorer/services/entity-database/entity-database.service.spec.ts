import { TestBed } from '@angular/core/testing';

import { EntityDatabaseService } from './entity-database.service';

describe('EntityDatabaseService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EntityDatabaseService = TestBed.get(EntityDatabaseService);
    expect(service).toBeTruthy();
  });
});
