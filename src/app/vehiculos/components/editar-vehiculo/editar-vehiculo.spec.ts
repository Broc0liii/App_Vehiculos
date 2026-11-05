import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EditarVehiculo } from './editar-vehiculo';

describe('EditarVehiculo', () => {
  let component: EditarVehiculo;
  let fixture: ComponentFixture<EditarVehiculo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditarVehiculo],
    }).compileComponents();

    fixture = TestBed.createComponent(EditarVehiculo);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
