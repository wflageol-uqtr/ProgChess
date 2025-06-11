import { Button } from "../../components/ui/button";
import AdminLayout from "../../components/layout/AdminLayout";
import { Link } from "react-router";
import { useEffect, useState } from "react";
import api from "../../utils/api";
import { DataTable } from "../../components/table/Data-table";
import { ExerciseColumns } from "../../components/table/ExerciseColumns";
import { handleApiError } from "../../utils/apiErrorHandler";

export default function ListExercise() {
  const [exercise, setExercise] = useState([]);

  const getAllExercise = async () => {
    try {
      const response = await api.get("/api/exercise");
      setExercise(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getAllExercise();
  }, []);

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col px-4 space-y-4 w-full mt-4">
        <div className="flex justify-between w-full">
          <h2 className="text-2xl font-semibold">Liste des exercices</h2>
          <Link to="/admin/exercise/create">
            <Button
              type="button"
              className="bg-green-500 hover:bg-green-700 cursor-pointer"
            >
              Ajouter
            </Button>
          </Link>
        </div>
        <DataTable columns={ExerciseColumns} data={exercise} filter="id" />
      </div>
    </AdminLayout>
  );
}
