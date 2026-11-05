import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ListaVehiculos } from './lista-vehiculos';

describe('ListaVehiculos', () => {
  let component: ListaVehiculos;
  let fixture: ComponentFixture<ListaVehiculos>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListaVehiculos],
    }).compileComponents();

    fixture = TestBed.createComponent(ListaVehiculos);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
