import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProductItemComponent } from './edit-product-item.component';

describe('EditProductItemComponent', () => {
  let component: EditProductItemComponent;
  let fixture: ComponentFixture<EditProductItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditProductItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditProductItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
