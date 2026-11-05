import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { ICatMarca } from '../../models/ICatMarca';
import { IMAGENES_VEHICULOS } from '../../constants/vehiculo-imagenes';

export interface NuevoVehiculo {
  year: string | null;
  modelo: string | null;
  catMarcaId: number | null;
}

@Component({
  selector: 'app-crear-vehiculo',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDividerModule,
  ],
  templateUrl: './crear-vehiculo.html',
  styleUrl: './crear-vehiculo.scss',
})
export class CrearVehiculo implements OnInit {
  private router = inject(Router);
  private form = inject(FormBuilder);
  private readonly http = inject(HttpClient);

  guardando = signal<boolean>(false);
  marcas = signal<ICatMarca[]>([]);
  readonly imagenes = IMAGENES_VEHICULOS;
  imagenSeleccionada = signal<string>(this.imagenes[0]);

  private readonly urlCatMarcas = '/api/catalogos/marcas';
  private readonly urlGuardarVehiculo = '/api/vehiculos';

  nuevoVehiculo = signal<NuevoVehiculo>({
    year: '',
    modelo: '',
    catMarcaId: 0,
  });

  formulario = this.form.group({
    year: ['', [Validators.required]],
    modelo: ['', [Validators.required]],
    catMarcaId: [0, [Validators.required, Validators.min(1)]],
  });

  ngOnInit(): void {
    this.obtenerMarcas().subscribe({
      next: (marcas) => {
        this.marcas.set(marcas);
      },
      error: (error) => {
        console.error('Error al obtener las marcas:', error);
      },
    });
  }

  obtenerMarcas() {
    return this.http.get<ICatMarca[]>(this.urlCatMarcas);
  }

  seleccionarImagen(img: string) {
    this.imagenSeleccionada.set(img);
  }

  guardarVehiculo() {
    if (this.guardando()) return;

    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    const valoresFormulario = this.formulario.getRawValue();
    this.nuevoVehiculo.set({
      ...valoresFormulario,
      catMarcaId: valoresFormulario.catMarcaId ? Number(valoresFormulario.catMarcaId) : 0,
    });

    this.guardando.set(true);
    this.formulario.disable();

    console.log('Nuevo Vehículo:', this.nuevoVehiculo());

    this.http.post(this.urlGuardarVehiculo, this.nuevoVehiculo()).subscribe({
      next: (response) => {
        console.log('Vehículo guardado exitosamente:', response);
        this.formulario.reset({ year: '', modelo: '', catMarcaId: 0 });
        this.guardando.set(false);
        this.formulario.enable();
        this.router.navigate(['/lista-vehiculos']);
      },
      error: (error) => {
        console.error('Error al guardar el vehículo:', error);
        this.guardando.set(false);
        this.formulario.enable();
      },
    });
  }
}
