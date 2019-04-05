import { TestBed } from '@angular/core/testing';

import { EntityDatabaseDatasource } from './entity-database-datasource.service';

describe('EntityDatabaseDatasourceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EntityDatabaseDatasource = TestBed.get(EntityDatabaseDatasource);
    expect(service).toBeTruthy();
  });
});
