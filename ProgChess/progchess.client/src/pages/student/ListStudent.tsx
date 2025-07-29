import { useEffect, useState } from "react";
import AdminLayout from "../../components/layout/AdminLayout";
import api from "../../utils/api";
import { handleApiError } from "../../utils/apiErrorHandler";
import { DataTable } from "../../components/table/Data-table";
import { StudentExerciseColumns } from "../../components/table/StudentExerciseColumns";
import type { StudentExerciseGroup } from "../../utils/type";

export default function ListStudent() {
  const [studentExercise, setStudentExercise] = useState<
    StudentExerciseGroup[]
  >([]);

  const gellAllStudent = async () => {
    try {
      const response = await api.get("/api/studentexercise/group/student");
      setStudentExercise(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    gellAllStudent();
  }, []);

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col px-4 space-y-4 w-full mt-4">
        <h2 className="font-semibold text-2xl">Liste des étudiants</h2>
        <DataTable
          data={studentExercise}
          columns={StudentExerciseColumns}
          apiRoute="/api/student"
          filter="permanentCode"
        />
      </div>
    </AdminLayout>
  );
}
