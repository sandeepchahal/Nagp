import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailProductItemComponent } from './detail-product-item.component';

describe('DetailProductItemComponent', () => {
  let component: DetailProductItemComponent;
  let fixture: ComponentFixture<DetailProductItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailProductItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailProductItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
