import AdminLayout from "../../components/layout/AdminLayout";

export default function StudentCode() {
  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col w-full space-y-4 mt-4 px-4">
        <div className="flex flex-col w-full">
          <h2 className="text-2xl font-semibold">Liste des étudiants</h2>
          <p>
            Cette section temporaire est l'emplacement où l'enseignant peut
            mettre les codes des étudiants autorisés à faire les exercices.
          </p>
        </div>
        <div className="flex h-screen flex-row w-full border rounded-lg">
          <div className="w-1/2">1</div>
          <div className="w-1/2">2</div>
        </div>
      </div>
    </AdminLayout>
  );
}
