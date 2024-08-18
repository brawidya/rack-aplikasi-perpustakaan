using System;
using System.Collections.Generic;

// Abstraksi
abstract class Item

    //encapsulation
{
    public string Judul { get; private set; }
    public string Penulis { get; private set; }
    public bool Dipinjam { get; private set; } = false;

    public Item(string judul, string penulis)
    {
        Judul = judul;
        Penulis = penulis;
    }

    public void SetJudul(string judul)
    {
        Judul = judul;
    }

    public void SetPenulis(string penulis)
    {
        Penulis = penulis;
    }

    public void Pinjam()
    {
        Dipinjam = true;
    }

    public void Kembali()
    {
        Dipinjam = false;
    }

    public abstract void TampilkanInfo(); // Metode abstrak (Polimorfisme)
}

// Enkapsulasi
class Buku : Item
{
    public int Halaman { get; private set; }

    public Buku(string judul, string penulis, int halaman)
        : base(judul, penulis)
    {
        Halaman = halaman;
    }

    public void SetHalaman(int halaman)
    {
        Halaman = halaman;
    }

    public override void TampilkanInfo()
    { // Polimorfisme
        string status = Dipinjam ? "Dipinjam" : "Tersedia";
        Console.WriteLine($"| Buku      | {Judul,-20} | {Penulis,-15} | {Halaman,10} Halaman | {status,-10} |");
    }
}

class Majalah : Item
{
    public int Edisi { get; private set; }

    public Majalah(string judul, string penulis, int edisi)
        : base(judul, penulis)
    {
        Edisi = edisi;
    }

    public void SetEdisi(int edisi)
    {
        Edisi = edisi;
    }

    public override void TampilkanInfo()
    { // Polimorfisme
        string status = Dipinjam ? "Dipinjam" : "Tersedia";
        Console.WriteLine($"| Majalah   | {Judul,-20} | {Penulis,-15} | Edisi {Edisi,10} | {status,-10} |");
    }
}

// Antarmuka (Polimorfisme)
interface IPinjam
{
    void PinjamItem();
}

// Kelas yang mengimplementasikan antarmuka
class BukuPinjam : Buku, IPinjam
{
    public BukuPinjam(string judul, string penulis, int halaman)
        : base(judul, penulis, halaman) { }

    public void PinjamItem()
    {
        if (!Dipinjam)
        {
            Pinjam();
            Console.WriteLine($"Buku \"{Judul}\" telah dipinjam.");
        }
        else
        {
            Console.WriteLine($"Buku \"{Judul}\" sudah dipinjam.");
        }
    }
}

class MajalahPinjam : Majalah, IPinjam
{
    public MajalahPinjam(string judul, string penulis, int edisi)
        : base(judul, penulis, edisi) { }

    public void PinjamItem()
    {
        if (!Dipinjam)
        {
            Pinjam();
            Console.WriteLine($"Majalah \"{Judul}\" telah dipinjam.");
        }
        else
        {
            Console.WriteLine($"Majalah \"{Judul}\" sudah dipinjam.");
        }
    }
}

// Kelas Perpustakaan untuk mengelola item
class Perpustakaan
{
    private List<Item> items = new List<Item>();

    public void TambahItem(Item item)
    {
        items.Add(item);
    }

    public void TampilkanSemuaItem()
    {
        Console.WriteLine("| Tipe      | Judul               | Penulis         | Info              | Status     |");
        Console.WriteLine(new string('-', 80));
        foreach (var item in items)
        {
            item.TampilkanInfo();
        }
        Console.WriteLine(new string('-', 80));
    }

    public void EditItem(string judul)
    {
        foreach (var item in items)
        {
            if (item.Judul.Equals(judul, StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Masukkan judul baru: ");
                string judulBaru = Console.ReadLine();
                Console.Write("Masukkan penulis baru: ");
                string penulisBaru = Console.ReadLine();
                item.SetJudul(judulBaru);
                item.SetPenulis(penulisBaru);
                if (item is Buku)
                {
                    Buku buku = item as Buku;
                    Console.Write("Masukkan jumlah halaman baru: ");
                    int halamanBaru = int.Parse(Console.ReadLine());
                    buku.SetHalaman(halamanBaru);
                }
                else if (item is Majalah)
                {
                    Majalah majalah = item as Majalah;
                    Console.Write("Masukkan nomor edisi baru: ");
                    int edisiBaru = int.Parse(Console.ReadLine());
                    majalah.SetEdisi(edisiBaru);
                }
                Console.WriteLine("Item berhasil diperbarui.");
                return;
            }
        }
        Console.WriteLine("Item tidak ditemukan.");
    }

    public void PinjamItem(string judul)
    {
        foreach (var item in items)
        {
            if (item.Judul.Equals(judul, StringComparison.OrdinalIgnoreCase) && item is IPinjam)
            {
                IPinjam itemPinjam = item as IPinjam;
                itemPinjam.PinjamItem();
                return;
            }
        }
        Console.WriteLine("Item tidak ditemukan atau tidak dapat dipinjam.");
    }

    public void HapusItem(string judul)
    {
        Item itemUntukDihapus = null;
        foreach (var item in items)
        {
            if (item.Judul.Equals(judul, StringComparison.OrdinalIgnoreCase))
            {
                itemUntukDihapus = item;
                break;
            }
        }
        if (itemUntukDihapus != null)
        {
            items.Remove(itemUntukDihapus);
            Console.WriteLine($"Item \"{judul}\" telah dihapus dari perpustakaan.");
        }
        else
        {
            Console.WriteLine("Item tidak ditemukan.");
        }
    }
}

// Kelas Antarmuka Pengguna untuk berinteraksi dengan sistem perpustakaan
class AntarmukaPengguna
{
    private Perpustakaan perpustakaan = new Perpustakaan();

    public void Mulai()
    {
        while (true)
        {
            Console.WriteLine("\nSistem Manajemen Perpustakaan");
            Console.WriteLine("1. Tambah Buku");
            Console.WriteLine("2. Tambah Majalah");
            Console.WriteLine("3. Tampilkan Semua Item");
            Console.WriteLine("4. Edit Item");
            Console.WriteLine("5. Pinjam Item");
            Console.WriteLine("6. Hapus Item");
            Console.WriteLine("7. Keluar");
            Console.Write("Pilih opsi: ");
            int pilihan = int.Parse(Console.ReadLine());

            switch (pilihan)
            {
                case 1:
                    TambahBuku();
                    break;
                case 2:
                    TambahMajalah();
                    break;
                case 3:
                    perpustakaan.TampilkanSemuaItem();
                    break;
                case 4:
                    EditItem();
                    break;
                case 5:
                    PinjamItem();
                    break;
                case 6:
                    HapusItem();
                    break;
                case 7:
                    return;
                default:
                    Console.WriteLine("Opsi tidak valid. Silakan coba lagi.");
                    break;
            }
        }
    }

    private void TambahBuku()
    {
        Console.Write("Masukkan judul: ");
        string judul = Console.ReadLine();
        Console.Write("Masukkan penulis: ");
        string penulis = Console.ReadLine();
        Console.Write("Masukkan jumlah halaman: ");
        int halaman = int.Parse(Console.ReadLine());

        BukuPinjam buku = new BukuPinjam(judul, penulis, halaman);
        perpustakaan.TambahItem(buku);
        Console.WriteLine("Buku berhasil ditambahkan.");
    }

    private void TambahMajalah()
    {
        Console.Write("Masukkan judul: ");
        string judul = Console.ReadLine();
        Console.Write("Masukkan penulis: ");
        string penulis = Console.ReadLine();
        Console.Write("Masukkan nomor edisi: ");
        int edisi = int.Parse(Console.ReadLine());

        MajalahPinjam majalah = new MajalahPinjam(judul, penulis, edisi);
        perpustakaan.TambahItem(majalah);
        Console.WriteLine("Majalah berhasil ditambahkan.");
    }

    private void EditItem()
    {
        Console.Write("Masukkan judul item yang ingin diedit: ");
        string judul = Console.ReadLine();
        perpustakaan.EditItem(judul);
    }

    private void PinjamItem()
    {
        Console.Write("Masukkan judul item yang ingin dipinjam: ");
        string judul = Console.ReadLine();
        perpustakaan.PinjamItem(judul);
    }

    private void HapusItem()
    {
        Console.Write("Masukkan judul item yang ingin dihapus: ");
        string judul = Console.ReadLine();
        perpustakaan.HapusItem(judul);
    }
}

class Program
{
    static void Main(string[] args)
    {
        AntarmukaPengguna antarmuka = new AntarmukaPengguna();
        antarmuka.Mulai();
    }
}