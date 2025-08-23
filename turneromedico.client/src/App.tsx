import { useEffect, useState } from 'react';
import './App.css';

interface Especialidad {
  id: number;
  nombre: string;
}

interface Medico {
  id: number;
  nombre: string;
  apellido: string;
  especialidades?: Especialidad[];
}

function App() {
  const [medicos, setMedicos] = useState<Medico[] | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    cargarMedicos();
  }, []);

  async function cargarMedicos() {
    try {
      const resp = await fetch('/api/Medicos');
      if (!resp.ok) throw new Error(`Error ${resp.status}`);
      const data: Medico[] = await resp.json();
      setMedicos(data);
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Error al obtener médicos');
    }
  }

  return (
    <div>
      <h1>Turnero Médico</h1>
      <p>Listado de médicos (desde la API).</p>
      {error && <p style={{ color: 'red' }}>Error: {error}</p>}
      {medicos === null ? (
        <p><em>Cargando...</em></p>
      ) : medicos.length === 0 ? (
        <p>No hay médicos cargados.</p>
      ) : (
        <table className="table" aria-label="Médicos">
          <thead>
            <tr>
              <th>Id</th>
              <th>Nombre</th>
              <th>Apellido</th>
              <th>Especialidades</th>
            </tr>
          </thead>
          <tbody>
            {medicos.map(m => (
              <tr key={m.id}>
                <td>{m.id}</td>
                <td>{m.nombre}</td>
                <td>{m.apellido}</td>
                <td>{(m.especialidades?.map(e => e.nombre) ?? []).join(', ')}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default App;