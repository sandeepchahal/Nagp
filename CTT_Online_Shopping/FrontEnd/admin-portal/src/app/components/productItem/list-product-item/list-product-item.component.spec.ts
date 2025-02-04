import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListProductItemComponent } from './list-product-item.component';

describe('ListProductItemComponent', () => {
  let component: ListProductItemComponent;
  let fixture: ComponentFixture<ListProductItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListProductItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListProductItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
